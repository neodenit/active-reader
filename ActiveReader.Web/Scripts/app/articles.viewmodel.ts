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
    selectedArticleTitle = ko.observable("");
    selectedArticleID = ko.observable(-1);

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
        this.selectedArticleID(article.id);
        this.selectedArticleTitle(article.title);

        var initialPosition = 0;
        this.getStartText(article.id, initialPosition);
    }

    public next() {
        var nextPosition = this.currentPosition() + 1;
        this.getStartText(this.selectedArticleID(), nextPosition);
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

    private getStartText(articleID: number, position: number) {
        $.getJSON("/api/questions/" + articleID + "/" + position).done((data) => {
            this.currentPosition(data.position);
            var startText = data.startingWords;
            this.startText(startText);
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
