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
            _TestCountService.getAll(filter).done(function (result) {
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
                data: 'studentName',
                sortable: false
            },
            {
                targets: 3,
                data: 'studentScoreSum',
                sortable: false
            },
            {
                targets: 4,
                data: 'sumScore',
                sortable: false,
            },
            {
                targets: 5,
                data: 'inspectedStatus',
                sortable: true,
            },
            {
                targets: 6,
                data: 'teacherName',
                sortable: false,
                defaultContent:"暂未阅卷"
            },
            {
                targets: 7,
                data: 'testTitle',
                sortable: false
            },
            {
                targets:8,
                data: 'beginTime',
                sortable: false
            },
            {
                targets: 9,
                data: 'endTime',
                sortable: false,
            },
            {
                targets: 10,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    if (data.isInspected === false && data.canInspect === true) {
                        return [
                            `   <button type="button" class="btn btn-sm bg-secondary inspect-TestCount" data-TestCount-id="${row.id}" data-toggle="modal" data-target="#TestCountInspectModal">`,
                            `       <i class="fas fa-pencil-alt"></i>阅卷`,
                            '   </button>',
                            `   <button type="button" class="btn btn-sm bg-danger check-TestCount" data-TestCount-id="${row.id}" data-toggle="modal" data-target="#TestCountCheckModal">`,
                            `       <i class="fas fa-trash"></i>查看结果`,
                            '   </button>'
                        ].join('');
                    } else {
                        return [
                            `   <button type="button" class="btn btn-sm bg-danger check-TestCount" data-TestCount-id="${row.id}" data-toggle="modal" data-target="#TestCountCheckModal">`,
                            `       <i class="fas fa-trash"></i>查看结果`,
                            '   </button>'
                        ].join('');
                    }
                    
                }
            }
        ]
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

    $(document).on('click', '.inspect-TestCount', function (e) {
        var TestCountId = $(this).attr("data-TestCount-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'TestCounts/Inspect?TestCountId=' + TestCountId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#TestCountInspectModal div.modal-content').html(content);
            },
            error: function (e) { }
        });
    });

    $(document).on('click', '.check-TestCount', function (e) {
        var TestCountId = $(this).attr("data-TestCount-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'TestCounts/Check?TestCountId=' + TestCountId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#TestCountCheckModal div.modal-content').html(content);
            },
            error: function (e) { }
        });
    });
   
    $(document).on('click', 'a[data-target="#TestCountCreateModal"]', (e) => {
        $('.nav-tabs a[href="#TestCount-details"]').tab('show')
    });

    abp.event.on('TestCount.Inspected', (data) => {
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
