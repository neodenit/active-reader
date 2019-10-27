var ArticlesViewModel = /** @class */ (function () {
    function ArticlesViewModel() {
        var _this = this;
        this.isAdding = ko.observable(false);
        this.articles = ko.observableArray();
        this.newArticleTitle = ko.observable("");
        this.newArticleText = ko.observable("");
        this.isSelected = ko.observable(false);
        this.selectedArticleTitle = ko.observable("");
        this.selectedArticleID = ko.observable(-1);
        this.startText = ko.observable("");
        this.currentPosition = ko.observable(0);
        this.variants = ko.observableArray();
        this.answer = "";
        this.score = ko.observable(0);
        this.scoreStyle = ko.observable("");
        this.open = function (article) {
            _this.isSelected(true);
            _this.selectedArticleID(article.id);
            _this.selectedArticleTitle(article.title);
            _this.score(0);
            var initialPosition = 0;
            _this.getQuestion(article.id, initialPosition);
        };
        this.remove = function (article) {
            $.ajax({
                url: app.dataModel.articlesUrl + article.id,
                type: "DELETE",
                context: _this,
            }).done(function (result) {
                _this.articles.remove(article);
            });
        };
        this.check = function (answer) {
            var score = _this.score();
            if (answer !== _this.answer) {
                _this.scoreStyle("wrong");
                _this.score(score - 1);
                _this.variants.remove(answer);
            }
            else {
                _this.variants.removeAll();
                _this.scoreStyle("right");
                _this.score(score + 1);
                _this.getQuestion(_this.selectedArticleID(), _this.currentPosition());
            }
        };
        var me = this;
        $.getJSON(app.dataModel.articlesUrl).done(function (data) {
            me.articles(data);
        });
    }
    ArticlesViewModel.prototype.add = function () {
        var me = this;
        var newArticle = {
            title: me.newArticleTitle(),
            text: me.newArticleText()
        };
        $.post(app.dataModel.articlesUrl, newArticle).done(function (article) {
            me.articles.push(article);
            me.isAdding(false);
            me.dropValues();
        });
    };
    ArticlesViewModel.prototype.cancel = function () {
        this.isAdding(false);
        this.dropValues();
    };
    ArticlesViewModel.prototype.backToList = function () {
        this.isSelected(false);
    };
    ArticlesViewModel.prototype.getQuestion = function (articleID, position) {
        var me = this;
        $.getJSON("/api/questions/" + articleID + "/" + position).done(function (data) {
            me.currentPosition(data.answerPosition);
            me.variants(data.variants);
            me.startText(data.startingWords);
            me.answer = data.answer;
        });
    };
    ArticlesViewModel.prototype.dropValues = function () {
        this.newArticleTitle("");
        this.newArticleText("");
    };
    return ArticlesViewModel;
}());
app.addViewModel({
    name: "Articles",
    bindingMemberName: "articles",
    factory: ArticlesViewModel,
});
//# sourceMappingURL=articles.viewmodel.js.map