(function ($) {
    var _PaperService = abp.services.app.papers,
        _$modal = $('#PaperCreateModal'),
        _$automodal = $('#PaperAutoCreateModal'),
        _$form = _$modal.find('form'),
        _$autoform = _$automodal.find('form'),
        _$table = $('#PapersTable');

    var _$PapersTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        ajax: function (data, callback, settings) {
            var filter = $('#PapersSearchForm').serializeFormToObject(true);
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;

            abp.ui.setBusy(_$table);
            _PaperService.getPapers(filter).done(function (result) {
                callback({
                    recordsTotal: result.totalCount,
                    recordsFiltered: result.totalCount,
                    data: result.items
                });
            }).always(function () {
                abp.ui.clearBusy(_$table);
            });
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$PapersTable.draw(false)
            }
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                sortable: true
            },
            {
                targets: 1,
                data: 'title',
                sortable: false,
            }, {
                targets: 2,
                data: 'score',
                sortable: false,
            },
            {
                targets: 3,
                data: 'creationTime',
                sortable: false,
            },
            {
                targets: 4,
                data: 'createUserId',
                sortable: false,
            },
            {
                targets: 5,
                data: 'createUserName',
                sortable: false,
            },
            {
                targets: 6,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-Paper" data-Paper-id="${row.id}" data-toggle="modal" data-target="#PaperEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> 编辑`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-Paper" data-Paper-id="${row.id}" data-Paper-name="${row.name}">`,
                        `       <i class="fas fa-trash"></i> 删除`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        var Paper = _$form.serializeFormToObject();
        Paper.examList = [];
        var _$examCheckboxes = _$form[0].querySelectorAll("input[name='exam']:checked");
        if (_$examCheckboxes) {
            for (var examIndex = 0; examIndex < _$examCheckboxes.length; examIndex++) {
                var _$examCheckbox = $(_$examCheckboxes[examIndex]);
                Paper.examList.push(_$examCheckbox.val());
            }
        }
        abp.ui.setBusy(_$modal);
        _PaperService.create(Paper).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info('新增试卷成功!');
            _$PapersTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    _$automodal.find('.auto-save-button').on('click', (e) => {
        e.preventDefault();
        var Paper = _$autoform.serializeFormToObject();
        abp.ui.setBusy(_$modal);
        _PaperService.autoCreate(Paper).done(function () {
            _$automodal.modal('hide');
            _$autoform[0].reset();
            abp.notify.info('自动生成试卷成功!');
            _$PapersTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$automodal);
        });
    });

    $(document).on('click', '.delete-Paper', function () {
        var PaperId = $(this).attr("data-Paper-id");
        var PaperName = $(this).attr('data-Paper-name');

        deletePaper(PaperId, PaperName);
    });

    function deletePaper(PaperId, PaperName) {
        abp.message.confirm(
            abp.utils.formatString(
               '是否删除?',
                PaperName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _PaperService.delete({
                        id: PaperId
                    }).done(() => {
                        abp.notify.info('删除成功！');
                        _$PapersTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-Paper', function (e) {
        var PaperId = $(this).attr("data-Paper-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Papers/EditModal?PaperId=' + PaperId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#PaperEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        });
    });

    $(document).on('click', 'a[data-target="#PaperCreateModal"]', (e) => {
        $('.nav-tabs a[href="#Paper-details"]').tab('show')
    });

    abp.event.on('Paper.edited', (data) => {
        _$PapersTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$PapersTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which === 13) {
            _$PapersTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
