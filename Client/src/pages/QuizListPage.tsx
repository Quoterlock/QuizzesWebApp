import { useState } from "react";
import GetListOfQuiz from "../models/apiManager";
import { QuizLayout } from "../layouts/QuizLayout";
import { Link } from "react-router-dom";

export default function QuizListPage() {
    const [items, setItems] = useState<QuizListItem[]>([])
    setItems(GetListOfQuiz(0, 0)) // get all

    return (<QuizLayout>
        <div>
            {items.map((item) =>
                <Link to={"/quiz/${item.id}"}>
                    <div className="block-style">
                        <h5>{item.title}</h5>
                        <p>{item.id}</p>
                    </div>
                </Link>
            )}
        </div>
    </QuizLayout>)
}