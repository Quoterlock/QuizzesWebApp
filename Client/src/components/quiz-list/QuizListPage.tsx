import { QuizLayout } from "../layout/QuizLayout";
import QuizList from "../shared/QuizList";
import { useContext, useEffect, useState} from "react";
import { AppContext } from "../../services/AppContext";
import { Link } from "react-router-dom";
import { SearchBox } from "../shared/SearchBox";

export default function QuizListPage() {
    const [items, setItems] = useState<QuizListItem[]>([])
    const [searchValue, setSearchValue] = useState<string>("")
    const {api} = useContext(AppContext)
    const userName = localStorage.getItem("current-username")

    useEffect(() => {
        if(searchValue == null || searchValue == ""){
            api.GetList(0,0)
            .then((values)=>setItems(values))
            .catch((error) => console.log(error))
        } else {
            api.SearchAsync(searchValue)
                .then((values)=>setItems(values))
                .catch((error) => console.log(error))
        }
    }, [api, searchValue])

    const onSearch = (value:string) => {
        setSearchValue(value)
    } 

    return (<QuizLayout>
        <div className="col-sm-12 col-md-9 col-lg-6 mx-auto">
            <SearchBox onSearch={onSearch}/>
            {
                userName &&
                <div className="d-grid">
                    <Link to="/new-quiz/" className="btn active-btn mb-3">+ Create</Link>
                </div>
            }
            <QuizList items={items}></QuizList>    
        </div>
    </QuizLayout>)
}