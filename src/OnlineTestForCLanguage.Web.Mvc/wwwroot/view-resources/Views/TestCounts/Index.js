(function ($) {
    var _TestCountService = abp.services.app.testCounts,
        _$modal = $('#TestCountCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#TestCountsTable');

    var _$TestCountsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        ajax: function (data, callback, settings) {
            var filter = $('#TestCountsSearchForm').serializeFormToObject(true);
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;

            abp.ui.setBusy(_$table);
            _TestCountService.getTestCounts(filter).done(function (result) {
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
                action: () => _$TestCountsTable.draw(false)
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
                data: 'studentId',
                sortable: false
            },
            {
                targets: 2,
                data: '',
                sortable: false
            },
            {
                targets: 3,
                data: '',
                sortable: false
            },
            {
                targets: 4,
                data: '',
                sortable: false,
            },
            {
                targets: 5,
                data: '',
                sortable: false,
            },
            {
                targets: 6,
                data: '',
                sortable: false
            },
            {
                targets:7,
                data: '',
                sortable: false
            },
            {
                targets: 8,
                data: '',
                sortable: false,
            },
            {
                targets: 9,
                data: '',
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
                        `   <button type="button" class="btn btn-sm bg-secondary edit-TestCount" data-TestCount-id="${row.id}" data-toggle="modal" data-target="#TestCountEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> Edit`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-TestCount" data-TestCount-id="${row.id}" data-TestCount-name="${row.name}">`,
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

        var TestCount = _$form.serializeFormToObject();
        abp.ui.setBusy(_$modal);
        _TestCountService.create(TestCount).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info('新增考题成功!');
            _$TestCountsTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.delete-TestCount', function () {
        var TestCountId = $(this).attr("data-TestCount-id");
        var TestCountName = $(this).attr('data-TestCount-name');

        deleteTestCount(TestCountId, TestCountName);
    });

    function deleteTestCount(TestCountId, TestCountName) {
        abp.message.confirm(
            abp.utils.formatString(
               '是否删除?',
                TestCountName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _TestCountService.delete({
                        id: TestCountId
                    }).done(() => {
                        abp.notify.info('删除成功！');
                        _$TestCountsTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-TestCount', function (e) {
        var TestCountId = $(this).attr("data-TestCount-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'TestCounts/EditModal?TestCountId=' + TestCountId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#TestCountEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        });
    });

    $(document).on('click', 'a[data-target="#TestCountCreateModal"]', (e) => {
        $('.nav-tabs a[href="#TestCount-details"]').tab('show')
    });

    abp.event.on('TestCount.edited', (data) => {
        _$TestCountsTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$TestCountsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which === 13) {
            _$TestCountsTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
