import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import 'bootstrap/dist/css/bootstrap.css';
import {
	createBrowserRouter,
	RouterProvider,
	Route,
} from "react-router-dom"
import QuizPage from './pages/QuizPage.tsx';


const router = createBrowserRouter([
  {
    path:"/",
    element: <QuizPage/>
  }
])

ReactDOM.createRoot(document.getElementById('root')!).render(
  <RouterProvider router={router}/>
)
