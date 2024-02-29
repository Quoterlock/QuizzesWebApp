interface IQuizApi {
    GetList(startIndex:number, endIndex:number):QuizListItem[]
    GetByIdAsync(id:string): Promise<QuizAndResults>
    SaveResultAsync(result:QuizResult):Promise<RequesResult>
    CreateNewQuiz(quiz:QuizItem): Promise<RequesResult>
}