(function ($) {
    var _examService = abp.services.app.exams,
        _$modal = $('#examEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var exam = _$form.serializeFormToObject();
       
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
