import CreateQuizForm from "../components/CreateQuizForm";
import { QuizLayout } from "../layouts/QuizLayout";

export default function NewQuizPage() {
    
    const onCreate = (quiz:QuizItem) => {
            // TODO: add to server here
    }
    
    return(<QuizLayout>
        <div className="col-lg-4 col-md-7 col-sm-12 mx-auto block-style">
            <CreateQuizForm/>
        </div>
    </QuizLayout>)
}