(function ($) {
    var _TestCountService = abp.services.app.testCounts,
        _$modal = $('#TestCountInspectModal'),
        _$form = _$modal.find('form');

    function InspectTest() {

        var request = {};
        var _$examIds = _$form[0].querySelectorAll("input[name='examIds']");
        request.detail_Exams = [];
        request.Id = $("#TestCountId").val();

        if (_$examIds) {
            for (var examid = 0; examid < _$examIds.length; examid++) {
                var _$examid = $(_$examIds[examid]).val();
                //获取当前学生选择的答案或者填写的内容
                var score = $("#Grade-" + _$examid).val();
                request.detail_Exams.push({ ExamId: _$examid, Score: score, });
            }
        }
        
        abp.message.confirm(
            abp.utils.formatString(
                '阅卷完成?'),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _TestCountService.inspect(request).done(function () {
                        _$modal.modal('hide');
                        _$form[0].reset();
                        abp.notify.info('人工阅卷成功!');
                        abp.event.trigger('TestCount.Inspected', request);
                        
                    }).always(function () {
                        abp.ui.clearBusy(_$form);
                    });
                }
            }
        );
    }
    $("#Inspect-Test").click(
        function (e) {
            e.preventDefault();
            InspectTest();
        }
    );
    
    _$modal.on('shown.bs.modal', function () {
        _$form.find('input[type=text]:first').focus();
    });
})(jQuery);
