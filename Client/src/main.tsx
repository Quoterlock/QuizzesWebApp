import './styles/AppStyles.css'
import ReactDOM from 'react-dom/client'
import 'bootstrap/dist/css/bootstrap.css';
import {
	createBrowserRouter,
	RouterProvider,
} from "react-router-dom"
import QuizPage from './components/quiz/QuizPage.tsx';
import QuizListPage from './components/quiz-list/QuizListPage.tsx';
import LoginPage from './components/authorization/LoginPage.tsx';
import UserProfilePage from './components/profile/UserProfilePage.tsx';
import EditUserProfilePage from './components/profile/EditUserProfilePage.tsx';
import RegisterPage from './components/authorization/RegisterPage.tsx';
import NewQuizPage from './components/quiz/create/NewQuizPage.tsx';
import { RemoveQuizPage } from './components/quiz/RemoveQuizPage.tsx';

const router = createBrowserRouter([
  {
    path:"/",
    element: <QuizListPage/>
  },
  {
    path:"/quiz/:id",
    element: <QuizPage/>
  },
  {
    path:"/new-quiz/",
    element: <NewQuizPage/>
  },
  {
    path:"/login/",
    element: <LoginPage/>
  },
  {
    path:"/register/",
    element: <RegisterPage/>
  },
  {
    path:"/profile/:id",
    element: <UserProfilePage/>
  },
  {
    path:"/profile",
    element: <UserProfilePage/>
  },
  {
    path:"/profile/edit",
    element: <EditUserProfilePage/>
  },
  {
    path:"/delete-quiz/:id",
    element: <RemoveQuizPage/>
  }
])

ReactDOM.createRoot(document.getElementById('root')!).render(
  <RouterProvider router={router}/>
)
