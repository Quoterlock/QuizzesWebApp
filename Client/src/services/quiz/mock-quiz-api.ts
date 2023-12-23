export class MockQuizApi implements IQuizApi {
    GetById(id: string, setItem: (item: QuizItem) => void): void {
        console.log("use mock")
        setItem(quiz)
    }

    GetList(startIndex: number, endIndex: number): QuizListItem[] {
        return quizzes
    }
    async GetByIdAsync(id: string): Promise<QuizItem> {
        return quiz
    }
}

const quiz:QuizItem = {
    id:"1",
    rate: 5,
    title: "Quiz 1",
    author: "AuthorName",
    authorId: "AuthorID",
    questions: [
        {
            text:"Question1",
            correctAnswerIndex: 1,
            options: [
                {text:"Answer1"},
                {text:"Answer2"},
                {text:"Answer3"},
                {text:"Answer4"},
            ]
        },
        {
            text:"Question2",
            correctAnswerIndex: 2,
            options: [
                {text:"Answer1"},
                {text:"Answer2"},
                {text:"Answer3"},
                {text:"Answer4"},
            ]
        },
        {
            text:"Question3",
            correctAnswerIndex: 3,
            options: [
                {text:"Answer1"},
                {text:"Answer2"},
                {text:"Answer3"},
                {text:"Answer4"},
            ]
        },
    ]
}

const quizzes:QuizListItem[] = [
    {
        id:"1",
        title:"Quiz1",
        rate:5,
        author:"AutorName",
        authorId:"AuthorID",
    },
    {
        id:"2",
        title:"Quiz2",
        rate:5,
        author:"AutorName",
        authorId:"AuthorID",
    },
    {
        id:"3",
        title:"Quiz3",
        rate:5,
        author:"AutorName",
        authorId:"AuthorID",
    },
    {
        id:"4",
        title:"Quiz4",
        rate:5,
        author:"AutorName",
        authorId:"AuthorID",
    },
    {
        id:"5",
        title:"Quiz5",
        rate:5,
        author:"AutorName",
        authorId:"AuthorID",
    },
]