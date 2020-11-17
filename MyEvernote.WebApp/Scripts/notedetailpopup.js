$(function () {
    $('#model_notedail').on('show.bs.modal', function (event) {
        var btn = $(event.relatedTarget);

        noteid = btn.data("note-id");
        $('#model_notedail_body').load("/Note/GetNoteText/" + noteid);

    })
});