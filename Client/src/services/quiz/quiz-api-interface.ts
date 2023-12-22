interface IQuizApi {
    GetList(startIndex:number, endIndex:number):QuizListItem[]
    GetById(id:string, setItem:(item:QuizItem)=>void):void
}