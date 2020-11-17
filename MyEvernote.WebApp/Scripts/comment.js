
    var noteid = -1;
    var modelCommentBodyId = '#model_comment_body';

    $(function () {
        $('#model_comment').on('show.bs.modal', function (event) {
            var btn = $(event.relatedTarget);

            noteid = btn.data("note-id");
            $('#model_comment_body').load("/Comment/ShowNoteComments/" + noteid);

        })
    });

    function doComment(btn, e, commentid, spanid) {
        var button = $(btn); //burada jqery ile gelen metotlar bunlar dönüştürmek lazım.
        var mode = button.data("edit-mode")

        if (e == "edit_clicked") {
            if (!mode) { //editmode false ise
        button.data("edit-mode", true); //edit-mode'un değerini true yaptık.
                button.removeClass("btn-warning");
                button.addClass("btn-success");
                var btnSpan = button.find("span");
                btnSpan.removeClass = ("glyhicon-edit");
                btnSpan.addClass = ("glyhicon-ok");
                $(spanid).addClass("editable"); //buraya bir css ekledim.
                $(spanid).attr("contenteditable", true);
                $(spanid).focus();
            }
            else {
        button.data("edit-mode", false); //edit-mode'un değerini true yaptık.
                button.addClass("btn-warning");
                button.removeClass("btn-success");
                var btnSpan = button.find("span");
                btnSpan.addClass = ("glyhicon-edit");
                btnSpan.removeClass = ("glyhicon-ok");
                $(spanid).removeClass("editable"); //buraya bir css ekledim.
                $(spanid).attr("contenteditable", false);

                var txt = $(spanid).text();
                $.ajax({
        method: "POST",
                    url: "/Comment/Edit/" + commentid,
                    data: {text: txt }

                }).done(function (data) {
                    if (data.result) {
        //yorumlar partial tekrar yüklenir
        $('#model_comment_body').load("/Comment/ShowNoteComments/" + noteid);
                    }
                    else {
        alert("Yorum Güncellenemedi.");
                    }
                }).fail(function () {
        alert("Sunucu ile bağlantı kurulamadı.");
                });
            }
        }
        else if (e == "delete_clicked") {
            var dialog_result = confirm("Yorum Silinsin mi?");
            if (!dialog_result)
                return false;

            $.ajax({
        method: "GET",
                url: "/Comment/Delete/" + commentid
            }).done(function (data) {
                if (data.result) {
        //yorumlar partial tekrar yükenir.
        $('#model_comment_body').load("/Comment/ShowNoteComments/" + noteid);
                }
                else {
        alert("Yorum silinemedi");
                }
            }).fail(function () {
        alert("Sunucu ile bağlantı kurulmadı");
            });

        }
        else if (e == "new_clicked") {
            var txt = $("#new_comment_text").val();

            $.ajax({
        method: "POST",
                url: "/Comment/Create",
                data: {text:txt,"noteid":noteid} //
            }).done(function (data) {
                if (data.result) {
        //yorumlar partial tekrar yükenir.
        $('#model_comment_body').load("/Comment/ShowNoteComments/" + noteid);
                }
                else {
        alert("Yorum eklenemedi");
                }
            }).fail(function () {
        alert("Sunucu ile bağlantı kurulmadı");
            });

        }
    }
