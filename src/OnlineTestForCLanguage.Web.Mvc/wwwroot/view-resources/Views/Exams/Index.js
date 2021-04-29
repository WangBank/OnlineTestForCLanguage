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
                sortable: true
            },
            {
                targets: 3,
                data: 'content',
                sortable: false
            },
            {
                targets: 4,
                data: 'score',
                sortable: false
            },
            {
                targets: 5,
                data: 'title',
                sortable: false,
            },
            {
                targets:6,
                data: 'creationTime',
                sortable: false,
            },
            {
                targets:7,
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

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        var exam = _$form.serializeFormToObject();
        abp.ui.setBusy(_$modal);
      
        var _$answers = _$form[0].querySelectorAll("input[name='AnswerContent']");
        var ExamType = $("#ExamType").val();
      
        exam.answers = [];
        if (_$answers) {
            for (var answer = 0; answer < _$answers.length; answer++) {
                var _$answer = $(_$answers[answer]);
                exam.answers.push({ Content: _$answer.val(), AnswerId: 'answerid' + answer });
            }
        }
        if (ExamType === "1") {
            var answerCorrects = _$form[0].querySelectorAll("input[name='answerName']:checked");
            if (answerCorrects.length === 0) {
                abp.notify.error('请至少选择一个选项为正确选项!');
                return;
            }
            var ids = [];
            for (var i = 0; i < answerCorrects.length; i++) {
                ids.push(answerCorrects[i].id);
            }
            exam.CorrectAnswerId = ids.join(",")
        } else if (ExamType !== "3") {
            exam.CorrectAnswerId = _$form[0].querySelectorAll("input[name='answerName']:checked")[0].id;
        }
        _examService.create(exam).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info('新增考题成功!');
            _$examsTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });
    _$form.find('.close-button').on('click', (e) => {
        window.location.href = "Exams";
    });

    $('#closePage').on('click', (e) => {
        window.location.href = "Exams";
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

    $("#ExamType").on('change',
        function () {
            var ExamType = $(this).val();
            $('#examContent').html('');
            $('#Title').val('')
            $('#Content').val('')
            $('#Explain').val('')
            //单选 多选 判断 简答
            switch (ExamType) {
                case "0":
                    $('#examContent').html(`
                             <table class="col-md-12">
                                        <tr align="center">
                                            <td width="15%">正确选项</td>
                                            <td>答案内容</td>
                                        </tr>
                                        <tr align="center">
                                            <td><input type="radio" id='answerid0' name="answerName" /></td>
                                            <td><input type="text" name="AnswerContent" class="form-control" required maxlength="80"></td>
                                        </tr>
                                        <tr align="center">
                                            <td><input type="radio" id='answerid1' name="answerName" /></td>
                                            <td> <input type="text" name="AnswerContent" class="form-control" required maxlength="80"></td>

                                        </tr>
                                        <tr align="center">
                                            <td><input type="radio" id='answerid2' name="answerName" /></td>
                                            <td> <input type="text" name="AnswerContent" class="form-control" required maxlength="80"></td>

                                        </tr>
                                        <tr align="center">
                                            <td><input type="radio" id='answerid3' name="answerName" /></td>
                                            <td> <input type="text" name="AnswerContent" class="form-control" required maxlength="80"></td>

                                        </tr>
                                    </table>
                        
                    `)
                    $('#score').val('5')
                    break;
                case "1":
                    $('#examContent').html(`
                              <table class="col-md-12">
                                        <tr align="center">
                                            <td width="15%">是否答案</td>
                                            <td>答案内容</td>
                                        </tr>
                                        <tr align="center">
                                            <td><input type="checkbox" id='answerid0' name="answerName" /></td>
                                            <td><input type="text" name="AnswerContent" class="form-control" required maxlength="80"></td>
                                        </tr>
                                        <tr align="center">
                                            <td><input type="checkbox" id='answerid1' name="answerName" /></td>
                                            <td> <input type="text" name="AnswerContent" class="form-control" required maxlength="80"></td>

                                        </tr>
                                        <tr align="center">
                                            <td><input type="checkbox" id='answerid2' name="answerName" /></td>
                                            <td> <input type="text" name="AnswerContent" class="form-control" required maxlength="80"></td>

                                        </tr>
                                        <tr align="center">
                                            <td><input type="checkbox" id='answerid3' name="answerName" /></td>
                                            <td> <input type="text" name="AnswerContent" class="form-control" required maxlength="80"></td>

                                        </tr>
                                    </table>
                     `)
                    $('#score').val('10')
                    break;
                case "2":
                    $('#examContent').html(`
<table class="col-md-12">
                                        <tr align="center">
                                            <td width="15%">正确选项</td>
                                            <td>答案内容</td>
                                        </tr>
                                        <tr align="center">
                                            <td><input type="radio" id='answerid0' name="answerName" /></td>
                                            <td><input readonly type="text" name="AnswerContent" class="form-control" value="正确" required maxlength="80"></td>
                                        </tr>
                                        <tr align="center">
                                            <td><input type="radio" id='answerid1' name="answerName" /></td>
                                            <td> <input value="错误" readonly type="text" name="AnswerContent" class="form-control" required maxlength="80"></td>
                                        </tr>
                                    </table>

                    `)
                    $('#score').val('5')
                    break;
                case "3":
                    $('#examContent').html(`<textarea type="text" name="AnswerContent" class="form-control" required maxlength="4000"></textarea>`)
                    $('#score').val('10')
                    break;
                default:
                    $('#examContent').html(`
                             <table class="col-md-12">
                                        <tr align="center">
                                            <td width="15%">正确选项</td>
                                            <td>答案内容</td>
                                        </tr>
                                        <tr align="center">
                                            <td><input type="radio" id='answerid0' name="answerName" /></td>
                                            <td><input type="text" name="AnswerContent" class="form-control" required maxlength="80"></td>
                                        </tr>
                                        <tr align="center">
                                            <td><input type="radio" id='answerid1' name="answerName" /></td>
                                            <td> <input type="text" name="AnswerContent" class="form-control" required maxlength="80"></td>

                                        </tr>
                                        <tr align="center">
                                            <td><input type="radio" id='answerid2' name="answerName" /></td>
                                            <td> <input type="text" name="AnswerContent" class="form-control" required maxlength="80"></td>

                                        </tr>
                                        <tr align="center">
                                            <td><input type="radio" id='answerid3' name="answerName" /></td>
                                            <td> <input type="text" name="AnswerContent" class="form-control" required maxlength="80"></td>

                                        </tr>
                                    </table>
                        
                    `)
                    $('#score').val('5')
                    break;
            }

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
    $(document).keyup(function (event) {
        switch (event.keyCode) {
            case 27:
                window.location.href = "Exams";
                break;
            case 96:
                window.location.href = "Exams";
                break;
        }
    });
   
})(jQuery);
