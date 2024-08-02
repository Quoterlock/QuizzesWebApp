interface IQuizApi {
    
    GetList(startIndex: number, endIndex: number): Promise<QuizListItem[]>
    SearchAsync(searchValue: string): Promise<QuizListItem[]>
    GetByIdAsync(id:string): Promise<QuizItem>
    SaveResultAsync(result:QuizResult):Promise<RequesResult>
    CreateNewQuiz(quiz:QuizItem): Promise<RequesResult>
    DeleteQuiz(id:string) : Promise<RequesResult>
}