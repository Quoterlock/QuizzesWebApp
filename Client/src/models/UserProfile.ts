type UserProfile = {
    id:string,
    displayName:string,
    owner:Owner
    //CreatedQuizzes: QuizListItem[],
    //CompletedQuizzes: QuizListItem[],
    createdQuizzesCount: number,
    completedQuizzesCount: number
}

type Owner = {
    id:string,
    username:string,
}