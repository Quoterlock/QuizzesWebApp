import { QuizLayout } from "../layouts/QuizLayout";
import QuizList from "../components/QuizList";
import { useContext} from "react";
import { AppContext } from "../services/app-context";
import { Link } from "react-router-dom";

export default function QuizListPage() {
    const {api} = useContext(AppContext)
    const itemsList = api.GetList(0,0) 

    return (<QuizLayout>
        <div className="col-sm-12 col-md-9 col-lg-6 mx-auto">
            <div className="d-grid">
                <Link to="/new-quiz/" className="btn active-btn mb-3">+ Create</Link>
            </div>
            <QuizList items={itemsList}></QuizList>    
        </div>
    </QuizLayout>)
}