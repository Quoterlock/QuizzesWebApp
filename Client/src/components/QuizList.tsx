import { Link } from "react-router-dom";

interface Props{
    items: QuizListItem[]
}

export default function QuizList({items}:Props) {
    return (        
    <div>
        {items.map((item,index) =>
            <Link to={`/quiz/${item.id}`}>
                <div key={index} className="block-style">
                    <h5>{item.title}</h5>
                    <p>{item.id}</p>
                </div>
            </Link>
        )}
    </div>
    )
}