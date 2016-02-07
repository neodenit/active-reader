interface IDataModel {
    articlesUrl: string
}

interface IViewModel {
}

interface IApp {
    addViewModel(viewModel: IViewModel): void
    dataModel: IDataModel
}

declare var app: IApp;
