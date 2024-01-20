import { Link } from "react-router-dom";

interface Props{
    items: QuizListItem[]
}

export default function QuizList({items}:Props) {
    return (<div>
        {items.map((item,index) =>
            <Link key={index} to={`/quiz/${item.id}`} className="styless-link">
                <div className="block-style mb-3">
                    <h5>{item.title}</h5>
                    <p>{`author: ${item.author}`}</p>
                </div>
            </Link>
        )}
    </div>)
}