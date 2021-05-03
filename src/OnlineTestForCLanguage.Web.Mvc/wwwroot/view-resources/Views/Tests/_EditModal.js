(function ($) {
    var _TestService = abp.services.app.tests,
        _TestCountService = abp.services.app.testCounts,
        _$modal = $('#TestEditModal'),
        _$form = _$modal.find('form'),
        _$countmodal = $('#TestStartModal'),
        _$countform = _$countmodal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var Test = _$form.serializeFormToObject();
       
        abp.ui.setBusy(_$form);
        _TestService.update(Test).done(function () {
            _$modal.modal('hide');
            abp.notify.info('保存成功!');
            abp.event.trigger('Test.edited', Test);
        }).always(function () {
            abp.ui.clearBusy(_$form);
        });
    }

    function ensureSubmit() {
        if (!_$countform.valid()) {
            return;
        }
        var request = {};
        var TestCount = _$countform.serializeFormToObject();
        var _$examSelectids = _$countform[0].querySelectorAll("input[name='examSelectids']");
        var _$examNoteSelectids = _$countform[0].querySelectorAll("input[name='examNotSelectids']");
        request.detail_Exams = [];
        request.TestId = TestCount.Id;
        if (_$examSelectids) {
            for (var examid = 0; examid < _$examSelectids.length; examid++) {
                var _$examid = $(_$examSelectids[examid]).val();
                //获取当前学生选择的答案或者填写的内容
                var answers = '';
                var query = "input[name='answerName-" + _$examid+"']:checked";
                var answerCorrects = _$countform[0].querySelectorAll(query);
                var ids = [];
                if (answerCorrects && answerCorrects.length !== 0) {
                    for (var i = 0; i < answerCorrects.length; i++) {
                        ids.push(answerCorrects[i].id);
                    }
                    answers = ids.join(",")
                }
                request.detail_Exams.push({ ExamId: _$examid, Answers: answers, });
            }
        }

        if (_$examNoteSelectids) {
            for (var examnoid = 0; examnoid < _$examNoteSelectids.length; examnoid++) {
                var _$examnoid = $(_$examNoteSelectids[examnoid]).val();
                //获取当前学生选择的答案或者填写的内容
                var answerinput = $("#simpleAnswerId-" + _$examnoid).val();
                request.detail_Exams.push({ ExamId: _$examnoid, Answers: answerinput, });
            }
        }
        
        abp.message.confirm(
            abp.utils.formatString(
                '是否交卷?'),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _TestCountService.create(request).done(function () {
                        _$countmodal.modal('hide');
                        _$countform[0].reset();
                        abp.notify.info('交卷成功!');
                        abp.event.trigger('Test.edited', Test);
                    }).always(function () {
                        abp.ui.clearBusy(_$countform);
                    });
                }
            }
        );
    }

    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });
    $("#ensure-submit").click(
        function (e) {
            e.preventDefault();
            ensureSubmit();
        }
    );
    
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
