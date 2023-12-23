import { createContext } from "react"
import { MockQuizApi } from "./quiz/mock-quiz-api"
//import { QuizApi } from "./quiz/quiz-api"

interface ContextType{
    api : IQuizApi
}

export const AppContext = createContext<ContextType>({
    api: new MockQuizApi
})