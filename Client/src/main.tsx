import './styles/AppStyles.css'
import ReactDOM from 'react-dom/client'
import 'bootstrap/dist/css/bootstrap.css';
import {
	createBrowserRouter,
	RouterProvider,
} from "react-router-dom"
import QuizPage from './pages/QuizPage.tsx';
import QuizListPage from './pages/QuizListPage.tsx';
import NewQuizPage from './pages/NewQuizPage.tsx';
import LoginPage from './pages/LoginPage.tsx';
import UserProfilePage from './pages/UserProfilePage.tsx';
import EditUserProfilePage from './pages/EditUserProfilePage.tsx';
import RegisterPage from './pages/RegisterPage.tsx';

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
  }
])

ReactDOM.createRoot(document.getElementById('root')!).render(
  <RouterProvider router={router}/>
)
