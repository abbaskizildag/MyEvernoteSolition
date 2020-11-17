$(function () {
    var noteids = [];
    $("div[data-note-id]").each(function (i, e) {
        noteids.push($(e).data("note-id")); //id'yi aldık. javasctip'te push ile alırız veriyi.
    });

    $.ajax({
        method: "POST",
        url: "/Note/GetLiked",
        data: { ids: noteids } //ids diye değişken oluşturdul bu notids alıyor


    }).done(function (data) {

        if (data.result != null && data.result.length > 0) {
            for (var i = 0; i < data.result.length; i++) {
                var id = data.result[i];
                var likedNote = $("div[data-note-id=" + id + "]");
                var btn = likedNote.find("button[data-liked]");
                var span = btn.find(".like-star");   // btn.children().first(); //bu layklanmış demejtir.

                btn.data("liked", true);
                span.removeClass("glyphicon-star-empty");
                span.addClass("glyphicon-star");
            }
        }
    }).fail(function () {
    });

    $("button[data-liked]").click(function () {
        var btn = $(this);
        var liked = btn.data("liked");
        var noteid = btn.data("note-id");
        var spanStar = btn.find("span.like-star");//span'ında like-star olan
        var spanCount = btn.find("span.like-count");

        $.ajax({
            method: "POST",
            url: "/Note/SetLikeState",
            data: { "noteid": noteid, "liked": !liked } //liked değerin tersini gönderiyoruz. like' ise unlike oluyor.
        }).done(function (data) {
            if (data.hasError) {
                alert(data.errorMessage);

            }
            else {
                liked = !liked;
                btn.data("liked", liked);
                spanCount.text(data.result);

                spanStar.removeClass("glyphicon-star-empty");
                spanStar.removeClass("glyphicon-star");

                if (liked) {
                    spanStar.addClass("glyphicon-star");

                }
                else {
                    spanStar.addClass("glyphicon-star-empty");
                }
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı");
        })
    });
});