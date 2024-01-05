import { useContext, useState } from "react";
import CreateQuizForm from "../components/CreateQuizForm";
import { QuizLayout } from "../layouts/QuizLayout";
import { AppContext } from "../services/app-context";
import { useNavigate } from 'react-router-dom';
import ErrorNotification from "../components/ErrorNotification";

export default function NewQuizPage() {
    const [postError, setPostErrorFlag] = useState<boolean>(false);
    const {api} = useContext(AppContext)
    const navigate = useNavigate();
    const onCreate = (quiz:QuizItem) => {
        api.CreateNewQuiz(quiz)
        .then((result)=>{
            console.log(result)
            if(result.code === 200) {
                navigate("/");
            } else {
                setPostErrorFlag(true);
            }
        })
        .catch((error) => {
            console.error("An error occured:", error)
            navigate('/error/new-quiz')
        })
    }
    
    return(<QuizLayout>
        <div className="col-lg-4 col-md-7 col-sm-12 mx-auto">
            <div className="mb-3">
                { postError && <ErrorNotification title="Server error" message="something goes wrong when post to the server"/> }
            </div>
            <CreateQuizForm onCreate={onCreate}/>
        </div>
    </QuizLayout>)
}