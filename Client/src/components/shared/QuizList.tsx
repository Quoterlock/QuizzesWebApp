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
        <div key={index} className="block-style mb-3">
            <div className="d-flex justify-content-between">
                <Link to={`/quiz/${item.id}`} className="styless-link">
                    <div className="d-flex mt-1">
                        <h5>{item.title}</h5>
                    </div>
                </Link>
                { item.author.owner.id === localStorage.getItem("current-user-id") &&
                <div>
                    <IconButton iconSrc="./src/assets/icons/delete-icon.png" onClick={()=>{hOnDelete(item.id)}}/>
                </div>
                }
            </div>
            <div className="d-flex">
                <p className="me-1 mb-0">Created by </p>
                <a className="action-link" href={`/profile/${item.author.owner.username}`}>{item.author.displayName}</a>
            </div>
        </div>
        )}
    </div>)
}