(function ($) {
    var _examService = abp.services.app.exams,
        _$modal = $('#examEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var exam = _$form.serializeFormToObject();
        var _$answers = _$form[0].querySelectorAll("input[name='AnswerContent']");
        var ExamType = $("#ExamEditType").val();
        console.log(ExamType)
        exam.answers = [];
        if (_$answers) {
            for (var answer = 0; answer < _$answers.length; answer++) {
                var _$answer = $(_$answers[answer]);
                exam.answers.push({ Content: _$answer.val(), AnswerId: 'answerid' + answer });
            }
        }
        if (ExamType === "1" || ExamType === "MulSelect" ) {
            var answerCorrects = _$form[0].querySelectorAll("input[name='answerName']:checked");
            if (answerCorrects.length === 0) {
                abp.notify.error('请至少选择一个选项为正确选项!');
                return;
            }
            var ids = [];
            for (var i = 0; i < answerCorrects.length; i++) {
                ids.push(answerCorrects[i].id);
            }
            exam.CorrectDetailIds = ids.join(",")
        } else if (ExamType !== "3" && ExamType !== "ShortAnswer") {
            exam.CorrectDetailIds = _$form[0].querySelectorAll("input[name='answerName']:checked")[0].id;
        } else if (ExamType === "3" || ExamType === "ShortAnswer") {
            exam.answers.push({ Content: $("#simpleAnswerId").val(), AnswerId: 'answerid' + answer });
        }

        abp.ui.setBusy(_$form);
        _examService.update(exam).done(function () {
            _$modal.modal('hide');
            abp.notify.info('保存成功!');
            abp.event.trigger('exam.edited', exam);
        }).always(function () {
            abp.ui.clearBusy(_$form);
        });
    }

    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });

    _$form.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            save();
        }
    });

    _$modal.on('shown.bs.modal', function () {
        _$form.find('input[type=text]:first').focus();
    });
})(jQuery);
