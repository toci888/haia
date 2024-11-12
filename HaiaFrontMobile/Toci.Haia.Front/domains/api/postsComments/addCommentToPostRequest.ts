import axios from "axios";
import { env } from "../../../env/env.dev";
import { endpoints } from "../config/apiConfig";

interface addCommentToPostRequestRqDto {
  userId: number, 
  postId: number, 
  content: string
}

export const addCommentToPostRequest = async ({ userId, content, postId }: addCommentToPostRequestRqDto): Promise<number> => {
  try {
    const response = await axios.post(`${env.baseUrl}${endpoints.addCommentToPost}`, {
      postId: postId,
      userId: userId,
      text: content,
    });
    console.log(response);
    return 1;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error('Błąd Axios:', {
          message: error.message,
          status: error.response?.status,
          data: error.response?.data,
      });
    } else {
      console.error('Inny błąd:', error);
    }
    console.error('Error addCommentToPostRequest:', error);
    return -1;
  }
};