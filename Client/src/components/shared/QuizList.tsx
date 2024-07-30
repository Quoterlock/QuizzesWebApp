import { Link, useNavigate } from "react-router-dom";
import { IconButton } from "./IconButton";

interface Props{
    items: QuizListItem[]
}

export default function QuizList({items}:Props) {
    const navigate = useNavigate();
    const hOnDelete = (id:string) => {
        navigate(`/delete-quiz/${id}`);
    }
    

    return (<div>
        {items.map((item,index) =>
        <div key={index} className="block-style mb-3 d-flex justify-content-between">
            <Link to={`/quiz/${item.id}`} className="styless-link">
                <div>
                    <h5>{item.title}</h5>
                    <p>{`author: ${item.author}`}</p>
                </div>
            </Link>   
            { item.authorId === localStorage.getItem("current-user-id") &&
                <div>
                    <IconButton iconSrc="./src/assets/icons/delete-icon.png" onClick={()=>{hOnDelete(item.id)}}/>
                </div>
            } 
        </div>
        )}
    </div>)
}