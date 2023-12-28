import { useContext } from "react";
import CreateQuizForm from "../components/CreateQuizForm";
import { QuizLayout } from "../layouts/QuizLayout";
import { AppContext } from "../services/app-context";

export default function NewQuizPage() {
    const {api} = useContext(AppContext)
    const onCreate = (quiz:QuizItem) => {
        api.CreateNewQuiz(quiz).then((result)=>{
            console.log(result)
        })
    }
    
    return(<QuizLayout>
        <div className="col-lg-4 col-md-7 col-sm-12 mx-auto">
            <CreateQuizForm onCreate={onCreate}/>
        </div>
    </QuizLayout>)
}