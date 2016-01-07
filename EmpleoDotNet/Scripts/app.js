function onJobOpportunityDetailClick(element) {
    var detailUrl = $(element).attr("data-url");
    if (detailUrl) {
        window.location = detailUrl;
    }
};

$(document).ready(function () {
    var quickSearchTemplate = [
        '<li class="quick-search-item">',
            '<div>',
                '<div class="info">',
                    '<div><span class="expand glyphicon glyphicon-plus"></span><span class="name"></span></div>',
                    '<span class="description hide"></span>',
                    '<a class="view-more" href="">Mas...</a> | <a href="" class="c-web-site">Web site</a>',
                '</div>',
            '</div>',
        '</li>'
    ].join(' ');
    var quickSearchResultContainer = $("#quick-search-result");

    $("#quick-search")
        .blur(function () {
            quickSearchResultContainer.hide();
        })
        .keyup(function () {
            var $ul = quickSearchResultContainer.show().find("ul");

            if (this.value == "") {
                quickSearchResultContainer.hide();
            }
            else {
                quickSearchResultContainer.show();
            }

            $.get(href("~/JobOpportunity/quickjobsearch"), { hint: this.value }, function (response) {

                if (response) {
                    $ul.find("li").remove();
                    for (var i = 0, total = response.length; i < total; i++) {
                        var current = response[i];

                        for (var y = 0; y < current.length; y++) {
                            var job = current[y];
                            var $element = $($.parseHTML(quickSearchTemplate));
                            $element.find(".category").text(job.categoryString);
                            $element.find(".description").text(job.description);
                            $element.find(".name").text(job.name);
                            $element.find(".c-web-site").attr("href", job.companyUrl);
                            $element.find(".view-more").attr("href", href("~/JobOpportunity/detail?id=" + job.id));
                            $element.find(".expand").hover(function () {
                                $element
                                    .find(".description")
                                    .toggleClass("hide");
                            });

                            $ul.append($element);
                        }
                    }
                }
            });
        });
})



