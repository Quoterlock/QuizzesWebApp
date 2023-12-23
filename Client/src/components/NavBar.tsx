import { ReactNode } from "react"
import { Link } from "react-router-dom";

export default function NavBar() {
    return(<div className="custom-navbar">
        <Link to="/" className="styless-link">
            <h5 className="text-center">QuizIt</h5>
        </Link>
    </div>)
}