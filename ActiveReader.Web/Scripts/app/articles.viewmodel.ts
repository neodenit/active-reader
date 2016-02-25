interface IArticle {
    id: number
    title: string
    text: string
}

class ArticlesViewModel {
    isAdding = ko.observable(false);
    articles = ko.observableArray<IArticle>();

    newArticleTitle = ko.observable("");
    newArticleText = ko.observable("");

    isSelected = ko.observable(false);
    selectedTitle = ko.observable("");
    startText = ko.observable("");
    currentPosition = ko.observable(0);

    constructor() {
        $.getJSON(app.dataModel.articlesUrl).done((data) => {
            this.articles(data);
        });
    }

    public add() {
        var newArticle = {
            title: this.newArticleTitle(),
            text: this.newArticleText()
        };

        $.post(app.dataModel.articlesUrl, newArticle).done((article: IArticle) => {
            this.articles.push(article);
            this.isAdding(false);
            this.dropValues();
        });
    }

    public cancel() {
        this.isAdding(false);
        this.dropValues();
    }

    public open(article: IArticle) {
        this.isSelected(true);
        this.selectedTitle(article.title);
        $.getJSON("/api/questions/" + article.id + "/0").done((data) => {
            this.currentPosition(data.position);
            var startText = data.startingWords;
            this.startText(startText);
        });
    }

    public next() {

    }

    public backToList() {
        this.isSelected(false);
    }

    public remove(article: IArticle) {
        $.ajax({
            url: app.dataModel.articlesUrl + article.id,
            type: "DELETE",
        }).done((result: IArticle) => {
            this.articles.remove(article);
        });
    }

    private dropValues() {
        this.newArticleTitle("");
        this.newArticleText("");
    }
}

app.addViewModel({
    name: "Articles",
    bindingMemberName: "articles",
    factory: ArticlesViewModel
});
