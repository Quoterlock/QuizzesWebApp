import { createContext } from "react"
import { MockQuizApi } from "./quiz/mock-quiz-api"
import { AuthorizationService } from "./authorization/authorization-api"
//import { QuizApi } from "./quiz/quiz-api"

interface ContextType{
    api : IQuizApi
    authorizationService : IAuthorizationService
}

export const AppContext = createContext<ContextType>({
    api: new MockQuizApi,
    authorizationService : new AuthorizationService
})