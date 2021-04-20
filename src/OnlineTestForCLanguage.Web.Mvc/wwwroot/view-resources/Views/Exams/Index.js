(function ($) {
    var _examService = abp.services.app.exams,
        _$modal = $('#ExamCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#ExamsTable');

    var _$examsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        ajax: function (data, callback, settings) {
            var filter = $('#ExamsSearchForm').serializeFormToObject(true);
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;

            abp.ui.setBusy(_$table);
            _examService.getExams(filter).done(function (result) {
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
                action: () => _$examsTable.draw(false)
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
                data: 'examType_Info',
                sortable: false
            },
            {
                targets: 2,
                data: 'difficulty_Info',
                sortable: false
            },
            {
                targets: 3,
                data: 'content',
                sortable: false
            },
            {
                targets: 4,
                data: 'title',
                sortable: false,
            },
            {
                targets: 5,
                data: 'creationTime',
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
                        `   <button type="button" class="btn btn-sm bg-secondary edit-exam" data-exam-id="${row.id}" data-toggle="modal" data-target="#examEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> Edit`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-exam" data-exam-id="${row.id}" data-exam-name="${row.name}">`,
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

        var exam = _$form.serializeFormToObject();
        abp.ui.setBusy(_$modal);
        _examService.create(exam).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info('新增考题成功!');
            _$examsTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.delete-exam', function () {
        var examId = $(this).attr("data-exam-id");
        var examName = $(this).attr('data-exam-name');

        deleteexam(examId, examName);
    });

    function deleteexam(examId, examName) {
        abp.message.confirm(
            abp.utils.formatString(
               '是否删除?',
                examName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _examService.delete({
                        id: examId
                    }).done(() => {
                        abp.notify.info('删除成功！');
                        _$examsTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-exam', function (e) {
        var examId = $(this).attr("data-exam-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'exams/EditModal?examId=' + examId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#examEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        });
    });

    $(document).on('click', 'a[data-target="#examCreateModal"]', (e) => {
        $('.nav-tabs a[href="#exam-details"]').tab('show')
    });

    abp.event.on('exam.edited', (data) => {
        _$examsTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$examsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which === 13) {
            _$examsTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
