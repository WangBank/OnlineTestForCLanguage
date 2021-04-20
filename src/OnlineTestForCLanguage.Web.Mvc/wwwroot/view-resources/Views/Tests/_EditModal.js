(function ($) {
    var _TestService = abp.services.app.tests,
        _$modal = $('#TestEditModal'),
        _$form = _$modal.find('form');

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
