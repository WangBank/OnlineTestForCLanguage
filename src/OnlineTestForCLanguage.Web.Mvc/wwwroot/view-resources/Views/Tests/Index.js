(function ($) {
    var _TestService = abp.services.app.tests,
        _$modal = $('#TestCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#TestsTable');

    var _$TestsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        ajax: function (data, callback, settings) {
            var filter = $('#TestsSearchForm').serializeFormToObject(true);
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;

            abp.ui.setBusy(_$table);
            _TestService.getTests(filter).done(function (result) {
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
                action: () => _$TestsTable.draw(false)
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
            },
            {
                targets: 2,
                data: 'creationTime',
                sortable: false,
            },
            {
                targets: 3,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-Test" data-Test-id="${row.id}" data-toggle="modal" data-target="#TestEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> Edit`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-Test" data-Test-id="${row.id}" data-Test-name="${row.name}">`,
                        `       <i class="fas fa-trash"></i> Delete`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    _$form.validate({
        rules: {
            Password: "required",
            ConfirmPassword: {
                equalTo: "#Password"
            }
        }
    });

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        var Test = _$form.serializeFormToObject();
        abp.ui.setBusy(_$modal);
        _TestService.create(Test).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info('新增试卷成功!');
            _$TestsTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.delete-Test', function () {
        var TestId = $(this).attr("data-Test-id");
        var TestName = $(this).attr('data-Test-name');

        deleteTest(TestId, TestName);
    });

    function deleteTest(TestId, TestName) {
        abp.message.confirm(
            abp.utils.formatString(
               '是否删除?',
                TestName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _TestService.delete({
                        id: TestId
                    }).done(() => {
                        abp.notify.info('删除成功！');
                        _$TestsTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-Test', function (e) {
        var TestId = $(this).attr("data-Test-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Tests/EditModal?TestId=' + TestId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#TestEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        });
    });

    $(document).on('click', 'a[data-target="#TestCreateModal"]', (e) => {
        $('.nav-tabs a[href="#Test-details"]').tab('show')
    });

    abp.event.on('Test.edited', (data) => {
        _$TestsTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$TestsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which === 13) {
            _$TestsTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
