import { createContext } from "react"
//import { QuizApi } from "./quiz/quiz-api"
import { MockQuizApi } from "./quiz/mock-quiz-api"

interface ContextType{
    api : IQuizApi
}

export const AppContext = createContext<ContextType>({
    api: new MockQuizApi
})