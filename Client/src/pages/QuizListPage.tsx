import GetListOfQuiz from "../models/apiManager";
import { QuizLayout } from "../layouts/QuizLayout";
import QuizList from "../components/QuizList";

export default function QuizListPage() {
    const itemsList = GetListOfQuiz(0,0);

    return (<QuizLayout>
        <QuizList items={itemsList}></QuizList>
    </QuizLayout>)
}