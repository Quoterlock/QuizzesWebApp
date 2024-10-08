type UserProfile = {
    id:string,
    displayName:string,
    owner:Owner,
    createdQuizzes: QuizListItem[],
    completedQuizzesCount: number
}

type Owner = {
    id:string,
    username:string,
}