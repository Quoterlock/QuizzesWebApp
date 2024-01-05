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
import NewQuizSuccessPage from './pages/NewQuizSuccessPage.tsx';

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
    path:"/new-quiz/success/",
    element : <NewQuizSuccessPage/>
  }
])

ReactDOM.createRoot(document.getElementById('root')!).render(
  <RouterProvider router={router}/>
)
