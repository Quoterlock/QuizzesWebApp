import { QuizLayout } from "../layout/QuizLayout";
import QuizList from "./QuizList";
import { useContext} from "react";
import { AppContext } from "../../services/AppContext";
import { Link } from "react-router-dom";

export default function QuizListPage() {
    const {api} = useContext(AppContext)
    const itemsList = api.GetList(0,0) 

    const userName = localStorage.getItem("current-username")

    return (<QuizLayout>
        <div className="col-sm-12 col-md-9 col-lg-6 mx-auto">
            {
                userName &&
                <div className="d-grid">
                    <Link to="/new-quiz/" className="btn active-btn mb-3">+ Create</Link>
                </div>
            }
            <QuizList items={itemsList}></QuizList>    
        </div>
    </QuizLayout>)
}