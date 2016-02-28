interface IArticle {
    id: number
    text: string
    title: string
}

interface IQuestion {
    answer: string
    answerPosition: number
    articleID: number
    startingWords: string
    variants: Array<string>
}

class ArticlesViewModel {
    public isAdding = ko.observable(false);
    public articles = ko.observableArray<IArticle>();

    public newArticleTitle = ko.observable("");
    public newArticleText = ko.observable("");

    public isSelected = ko.observable(false);
    public selectedArticleTitle = ko.observable("");
    public selectedArticleID = ko.observable(-1);

    public startText = ko.observable("");
    public currentPosition = ko.observable(0);

    public variants = ko.observableArray<string>();
    public answer = "";

    public score = ko.observable(0);
    public scoreStyle = ko.observable("");

    constructor() {
        var me = this;

        $.getJSON(app.dataModel.articlesUrl).done((data: Array<IArticle>) => {
            me.articles(data);
        });
    }

    public add() {
        var me = this;

        var newArticle = {
            title: me.newArticleTitle(),
            text: me.newArticleText()
        };

        $.post(app.dataModel.articlesUrl, newArticle).done((article: IArticle) => {
            me.articles.push(article);
            me.isAdding(false);
            me.dropValues();
        });
    }

    public cancel() {
        this.isAdding(false);
        this.dropValues();
    }

    public open = (article: IArticle) => {
        this.isSelected(true);
        this.selectedArticleID(article.id);
        this.selectedArticleTitle(article.title);

        this.score(0);
        var initialPosition = 0;
        this.getQuestion(article.id, initialPosition);
    }

    public backToList() {
        this.isSelected(false);
    }

    public remove = (article: IArticle) => {
        $.ajax({
            url: app.dataModel.articlesUrl + article.id,
            type: "DELETE",
            context: this,
        }).done((result: IArticle) => {
            this.articles.remove(article);
        });
    }
    public check = (answer: string) => {
        var score = this.score();

        if (answer !== this.answer) {
            this.scoreStyle("wrong");
            this.score(score - 1);
            this.variants.remove(answer);
        } else {
            this.variants.removeAll();
            this.scoreStyle("right");
            this.score(score + 1);
            this.getQuestion(this.selectedArticleID(), this.currentPosition());
        }
    }
    private getQuestion(articleID: number, position: number) {
        var me = this;

        $.getJSON(`/api/questions/${articleID}/${position}`).done((data) => {
            me.currentPosition(data.answerPosition);
            me.variants(data.variants);
            me.startText(data.startingWords);
            me.answer = data.answer;
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
    factory: ArticlesViewModel,
});
