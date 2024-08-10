type QuizItem = {
    id: string
    rate: number,
    title: string,
    creationDate:string,
    author?:UserProfileInfo,
    questions: QuestionItem[],
    results?: QuizResult[]
}