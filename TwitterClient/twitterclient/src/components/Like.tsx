import { useEffect, useState } from "react";
import { Like } from "../models/like.model";

const LikeComponent = ( props: {postId: number, userId: number}) => {

    const [likes, setLikes] = useState<Like[]>([]);
    const [liked, setLiked] = useState(false);
    const [likeNumber, setLikeNumber] = useState(0);
    const [userId, setUserId] = useState(props.userId);
    const [postId, setPostId] = useState(props.postId);

    useEffect(() => {
        (
          async () => {
            const response = await fetch(`https://localhost:7082/Like/GetLikesByPostId/${encodeURIComponent(props.postId)}`, {
                headers: { 'Content-Type': 'application/json' },
                credentials: 'include',
                mode: 'cors',
            });

            const likeList : Like[] = await response.json();
            setLikes(likeList);
            setLikeNumber(likeList.length);
            likes.forEach(like => {
                if(like.userId === props.userId){
                    setLiked(true);
                    return;
                }
            });

          }
        )();
    });

    const likeUnlike = () => {
        if(liked){
            setLikeNumber(likeNumber-1);
            setLiked(!liked);
            setUserId(userId);
            unLikePost();
        } else{
            setLikeNumber(likeNumber+1);
            setLiked(!liked);
            setUserId(userId);
            likePost();
        }
    };

    const likePost = async () => {
        await fetch('https://localhost:7082' + '/Like/Like', {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            credentials: 'include',
            body: JSON.stringify({
                userId,
                postId
            }),
            mode: 'cors'
        });
    };

    const unLikePost = async () => {
        await fetch('https://localhost:7082' + '/Like/Unlike', {
            method: 'DELETE',
            headers: {'Content-Type': 'application/json'},
            credentials: 'include',
            body: JSON.stringify({
                userId,
                postId
            }),
            mode: 'cors'
        });
    };

    return(
        <div className="like-content">
            <label className="likes">{likeNumber}</label>
            <div>
                {liked ? 
                (<div className="like-button" onClick={likeUnlike}>
                    <i className="bi bi-hand-thumbs-up-fill"></i>
                </div>) : 
                (<div className="like-button" onClick={likeUnlike}>
                    <i className="bi bi-hand-thumbs-up"></i>
                </div>)}
            </div>
        </div>
    );
};

export default LikeComponent;