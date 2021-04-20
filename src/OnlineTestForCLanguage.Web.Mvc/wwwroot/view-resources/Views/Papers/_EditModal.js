(function ($) {
    var _PaperService = abp.services.app.papers,
        _$modal = $('#PaperEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var Paper = _$form.serializeFormToObject();
       
        abp.ui.setBusy(_$form);
        _PaperService.update(Paper).done(function () {
            _$modal.modal('hide');
            abp.notify.info('保存成功!');
            abp.event.trigger('Paper.edited', Paper);
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
