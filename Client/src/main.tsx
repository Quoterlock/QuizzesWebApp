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
  }
])

ReactDOM.createRoot(document.getElementById('root')!).render(
  <RouterProvider router={router}/>
)
