interface IRatesApi {
    RateQuiz(quizId:string, rate:number): Promise<RequesResult>
}