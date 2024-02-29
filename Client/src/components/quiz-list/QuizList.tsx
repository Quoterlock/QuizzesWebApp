import { Link, useNavigate } from "react-router-dom";
import { Button } from "../shared/Button";
import { MouseEvent } from "react";
import { IconButton } from "../shared/IconButton";

interface Props{
    items: QuizListItem[]
}

export default function QuizList({items}:Props) {
    const navigate = useNavigate();
    const hOnEdit = (id:string) => {
        navigate(`/remove-quiz/${id}`);
    }
    

    return (<div>
        {items.map((item,index) =>
        <div key={index} className="block-style mb-3">
            <Link to={`/quiz/${item.id}`} className="styless-link d-flex justify-content-between">
                <div>
                    <h5>{item.title}</h5>
                    <p>{`author: ${item.author}`}</p>
                </div>
                { item.authorId === localStorage.getItem("current-user-id") &&
                <div>
                    <IconButton iconSrc="./src/assets/icons/delete-icon.png" onClick={()=>{hOnEdit(item.id)}}/>
                </div>
                }
            </Link>    
        </div>
        )}
    </div>)
}