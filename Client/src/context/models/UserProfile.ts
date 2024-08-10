type UserProfile = {
    id:string,
    displayName:string,
    owner:Owner,
    createdQuizzes: QuizListItem[],
    completedQuizzesCount: number
    image?: File
}

type Owner = {
    id:string,
    username:string,
}