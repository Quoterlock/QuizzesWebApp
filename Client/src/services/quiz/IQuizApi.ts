interface IQuizApi {
    GetList(startIndex:number, endIndex:number):QuizListItem[]
    GetById(id:string, setItem:(item:QuizItem)=>void):void
    GetByIdAsync(id:string): Promise<QuizItem>
    CreateNewQuiz(quiz:QuizItem): Promise<RequesResult>
}