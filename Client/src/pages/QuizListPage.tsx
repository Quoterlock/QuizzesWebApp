import { QuizLayout } from "../layouts/QuizLayout";
import QuizList from "../components/QuizList";
import { useContext } from "react";
import { AppContext } from "../services/app-context";

export default function QuizListPage() {
    const {api} = useContext(AppContext)
    const itemsList = api.GetList(0,0) 

    return (<QuizLayout>
        <QuizList items={itemsList}></QuizList>
    </QuizLayout>)
}